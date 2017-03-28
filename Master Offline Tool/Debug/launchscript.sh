#cd Master\ Offline\ Tool/Debug


declare -a combinationsToUse=("Key" "Pos" "Rot" "KeyPos" "KeyRot" "PosRot" "KeyPosRot")

declare -a hiddenLayers=(
	10 5 0 0 0
	20 10 5 0 0
	40 30 20 10 0
	100 70 50 30 10
)

declare -a hiddenLayerCount=(
	2
	3
	4
	5
)

for combination in "${combinationsToUse[@]}"
do
	layerLength=${#hiddenLayerCount[@]}
	for ((i = 0; i<${layerLength}; i++))
	do
		echo "running setting: " $combination ${hiddenLayerCount[i]} ${hiddenLayers[i*5]} ${hiddenLayers[i*5+1]} ${hiddenLayers[i*5+2]} ${hiddenLayers[i*5+3]} ${hiddenLayers[i*5+4]}
		./master\ Offline\ Tool.exe $combination ${hiddenLayerCount[i]} ${hiddenLayers[i*5]} ${hiddenLayers[i*5+1]} ${hiddenLayers[i*5+2]} ${hiddenLayers[i*5+3]} ${hiddenLayers[i*5+4]}
	done
done
