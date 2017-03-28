#cd Master\ Offline\ Tool/Debug

declare -a entriesToAvrage=(
	60 
	20 
	240
)
declare -a entriesToMerge=(
	6
	3 
	20 
	40
)

for avrage in "${entriesToAvrage[@]}"
do
	for merge in "${entriesToAvrage[@]}"
	do
		rm -R SavednetSettings/.
		echo "running still with setting: " $avrage $merge
		#./DataStill64.exe $avrage $merge
		#sh ./launchscript.sh
		cd ..
		mkdir -p "SavedNetSettingsDifferentStills/$avrage $merge"
		cp -R SavedNetSettings/. "SavedNetSettingsDifferentStills/$avrage $merge"
		cd Debug
	done
done