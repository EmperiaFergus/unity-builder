echo "###########################"
echo "#         Patchkit        #"
echo "###########################"
#find / -type d -print
#cd ./test-project/Assets/Builders/patchkit/
echo 1.
echo $patchkitSecret
echo 2.
echo $patchkitAPIKey
echo 3.
echo $BUILD_PATH_FULL

for entry in "$search_dir"/*
do
  echo "$entry"
done
#chmod, +x, ./patchkit-tools
./test-project/Assets/Builders/patchkit/patchkit-tools.sh make-version -s $patchkitSecret -a $patchkitAPIKey -l test3 -f $BUILD_PATH_FULL -x

#set -e

## Figure out where this script is located.
#SELFDIR="/github/workspace/dist/patchkit"
#SELFDIR="`cd \"$SELFDIR\" && pwd`"
#
## Tell Bundler where the Gemfile and gems are.
#export BUNDLE_GEMFILE="$SELFDIR/vendor/Gemfile"
#unset BUNDLE_IGNORE_CONFIG
#
## Run the actual app using the bundled Ruby interpreter, with Bundler activated.
#exec "$SELFDIR/ruby/bin/ruby" -rbundler/setup "$SELFDIR/app/core/bootstrap.rb" "$@"
