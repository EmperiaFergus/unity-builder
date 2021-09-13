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
#update path to where new path is
#get access to patchkit
#./test-project/Assets/Builders/patchkit/patchkit-tools.sh make-version -s $patchkitSecret -a $patchkitAPIKey -l test3 -f $BUILD_PATH_FULL -x
# ADD patchkit /patchkit
# RUN chmod -R +x /patchkit
#/github/workspace/dist/patchkit/patchkit-tools.sh make-version -s $patchkitSecret -a $patchkitAPIKey -l test3 -f $BUILD_PATH_FULL -x

cd /patchkit
patchkit-tools.sh make-version -s $patchkitSecret -a $patchkitAPIKey -l test3 -f $BUILD_PATH_FULL -x


