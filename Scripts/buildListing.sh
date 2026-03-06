#!/bin/bash
OWNER="xaeky"
REPO="libertas-vr-utils"
SCRIPT_CWD=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
PACKAGES_NAMES='[
  "cloud.xaeky.unity-avatarworkspacesetup"  
]'
PACKAGES_PATH="$SCRIPT_CWD/../Packages"
PACKAGES='[]'
# Get from ./listing.json
LISTING=$(cat "$SCRIPT_CWD/listing.json" 2>/dev/null || echo '{"packages": {}}')
WEBSITE_PATH="$SCRIPT_CWD/../Website"

if ! command -v jq >/dev/null 2>&1; then
  echo "Error: jq is not installed."
  exit 1
fi

# List packages from "/Packages" directory, read manifest file "package.json" and save them to $PACKAGES
for package_name in $(echo "$PACKAGES_NAMES" | jq -r '.[]'); do
  manifest_path="$PACKAGES_PATH/$package_name/package.json"
  if [[ -f "$manifest_path" ]]; then
    package_info=$(jq -c '.' "$manifest_path")
    # Only push to PACKAGES the following fields: id, displayName, tagPrefix
    package_info=$(echo "$package_info" | jq -c '{id: .name, displayName: .displayName, tagPrefix: .tag}')
    PACKAGES=$(echo "$PACKAGES" | jq --argjson info "$package_info" '. + [$info]')
  else
    echo "Warning: Manifest file not found for package '$package_name' at path '$manifest_path'. Skipping."
  fi
done

if [[ -z "$PACKAGES" || "$PACKAGES" == "[]" ]]; then
  echo "Error: No valid packages found. Exiting."
  exit 1
fi

while read -r obj; do
  p_id=$(echo "$obj" | jq -r '.id')
  p_displayName=$(echo "$obj" | jq -r '.displayName')
  p_tagPrefix=$(echo "$obj" | jq -r '.tagPrefix')
  # Fetch tags for this package
  tags=$(curl -s "https://api.github.com/repos/$OWNER/$REPO/tags" | jq -r '.[] | select(.name | startswith("'"$p_tagPrefix"'")) | .name')
  versions="{}"
  # Fetch releases for each tag
  for tag in $tags; do
    version_number=$(echo "$tag" | sed "s/^$p_tagPrefix-//")
    version_release=$(curl -s "https://api.github.com/repos/$OWNER/$REPO/releases/tags/$tag")
    # Look for .zip asset url
    zip_url=$(echo "$version_release" | jq -r '.assets[] | select(.name | endswith(".zip")) | .browser_download_url')
    # Read the package.json asset.
    package_json_url=$(echo "$version_release" | jq -r '.assets[] | select(.name == "package.json") | .browser_download_url')
    package_json=$(curl -sL "$package_json_url")
    # Sanitize json by removing unused fields (localPath, legacyFolders, legacyFiles), and adding zipSHA256 and url (zip_url)
    zip_sha256=$(echo "$version_release" | jq -r '.assets[] | select(.name | endswith(".zip")) | .digest' | sed 's/^sha256://')
    package_json=$(echo "$package_json" | jq --arg sha "$zip_sha256" --arg url "$zip_url" 'del(.localPath, .legacyFolders, .legacyFiles) | . + {zipSHA256: $sha, url: $url}')
    versions=$(echo "$versions" | jq --arg v "$version_number" --argjson info "$package_json" '. + {($v): $info}')
  done
  echo "Processed package: $p_id with versions: $(echo "$versions" | jq -r 'keys[]')"
  LISTING=$(echo "$LISTING" | jq --arg id "$p_id" --argjson versions "$versions" '.packages += {($id): {versions: $versions}}')
done < <(echo "$PACKAGES" | jq -c '.[]')

# Output JSON listing, create the file if it doesn't exist
echo "$LISTING" | jq '.' > "$WEBSITE_PATH/index.json"