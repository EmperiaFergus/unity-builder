echo "###########################"
echo "#         Patchkit        #"
echo "###########################"
#find / -type d -print
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

rvm fix-permissions

rvm reinstall 2.1.2
ruby --version
cd /patchkit
/patchkit/patchkit-tools.sh make-version -s $patchkitSecret -a $patchkitAPIKey -l test3 -f $BUILD_PATH_FULL -x


