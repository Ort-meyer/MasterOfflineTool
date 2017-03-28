#cd Master\ Offline\ Tool/Debug

declare -a entriesToAvrage=(10 60 240)
declare -a entriesToMerge=(3 5 20 40)

for avrage in "${entriesToAvrage[@]}"
do
	for merge in "${entriesToAvrage[@]}"
	do
		echo "running still with setting: " $avrage $merge
		./DataStill64.exe $avrage $merge
		sh ./launchscript.sh
	done
done