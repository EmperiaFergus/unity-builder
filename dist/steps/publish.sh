echo "###########################"
echo "#         Patchkit        #"
echo "###########################"
find / -type d -print
cd /github/workspace/dist/steps/patchkit
ls -a
 ./patchkit-tools
#chmod, +x, ./patchkit-tools
patchkit-tools make-version -s $patchkitSecret -a $patchkitAPIKey -l "test upload" -f $BUILD_PATH_FULL

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
