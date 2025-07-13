#!/bin/sh
echo -ne '\033c\033]0;protonic\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/protonic.x86_64" "$@"
