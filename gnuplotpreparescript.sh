
baseLayers="5  100 70 50 30 10" # Don't forget, TWO SPACES between first two numbers
baseConfig="PosRot"
baseStill="60 20"

# Combination variation first
#target="Master Offline Tool/SavedNetSettingsDifferentStills/*/"
# Enter directory where we have all different stills
cd "Master Offline Tool/SavedNetSettingsDifferentStills/"
#echo $baseStill
# Enter still
cd "${baseStill}"
# Iterate over every combination
for d2 in */; do
	#echo "${d2}"
	cd "${d2}"
	# Enter folder where net results are
	cd "1110/"
	# Iterate over every network
	for f in *; do
		# Only grab the right network
		if [[ ${f} == *"${baseLayers}"* ]]; then
			#echo "${f}"
			cp "${f}" "../../../../../Gnuplot result graph data/Combination varied" #something like this?
			fi
	done
	# All files read, jump out to 1110 and then to where all combinations are
	cd "../../"
done
cd "../"
#echo $destination "===going into===" $target


# Layer variation second
cd "${baseStill}"
cd "${baseConfig}"
cd "1110/"
for f in *; do
	cp "${f}" "../../../../../Gnuplot result graph data/Layers varied"
done
# Return to pwd
cd "../../.."


# Vary still last
for d in */; do
	cd "${d}"
	cd "${baseConfig}"
	cd "1110/"
	for f in *; do
		# Only grab the right network
		if [[ ${f} == *"${baseLayers}"* ]]; then
			#echo "${f}"
			mkdir -p "../../../../../Gnuplot result graph data/Datastill varied/${d}"
			cp "${f}" "../../../../../Gnuplot result graph data/Datastill varied/${d}" #something like this?
			fi
	done
	cd "../../../"
done

exit 1


# Sortof general implementation that just gets all networks
#target="Master Offline Tool/SavedNetSettingsDifferentStills/*/"
# Enter directory where we have all different stills
cd "Master Offline Tool/SavedNetSettingsDifferentStills/"
for d in */; do # Should only be one folder here
	echo $d
	# Enter still
    cd "${d}"
    # Iterate over every combination
    for d2 in */; do
    	echo "${d2}"
    	cd "${d2}"
    	# Enter folder where net results are
    	cd "1110/"
    	# Iterate over every network
    	for f in *; do
    		echo "${f}" #For now we only print the network. Make sure copy is done!
    		cp "${f}" "../../../../../Gnuplot result graph data/Combination varied" #something like this?
    	done
    	# All files read, jump out to 1110 and then to where all combinations are
    	cd "../../"
    done
    cd "../"
done
#echo $destination "===going into===" $target