#cd Master\ Offline\ Tool/Debug

declare -a entriesToAvrage=(
	20 
	120
)
declare -a entriesToMerge=(
	5
	20
	10
)

for avrage in "${entriesToAvrage[@]}"
do
	for merge in "${entriesToMerge[@]}"
	do
		# Inform console what's happening
		echo "running still with setting: " $avrage $merge
		# Run the still for this setting
		./DataStill64.exe $avrage $merge
		# Launch the actual tool
	    sh ./launchscript.sh "$avrage $merge"
		## Start working on saving results for latest still
		cd ..
		# Create new directory for this still
		mkdir -p "SavedNetSettingsDifferentStills/$avrage $merge"
		# Move results
		mv SavedNetSettings/* "SavedNetSettingsDifferentStills/$avrage $merge"
		# Copy a certain file back so git doesn't bug out
		mv "SavedNetSettingsDifferentStills/$avrage $merge/allActivationFunctions.bigrun" SavedNetSettings
		# Reset folder for next run
		cd Debug
	done
done