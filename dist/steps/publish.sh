echo "###########################"
echo "#         Patchkit        #"
echo "###########################"
find / -type d -print
cd /github/workspace/test-project/Assets/builders/patchKit/
#chmod, +x, ./patchkit-tools
patchkit-tools.sh make-version -s $patchkitSecret -a $patchkitAPIKey -l test2 -f $BUILD_PATH_FULL -x

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
