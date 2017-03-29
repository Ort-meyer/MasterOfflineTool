#cd Master\ Offline\ Tool/Debug

dataStill=$1
#declare -a combinationsToUse=("Key" "Pos" "Rot" "KeyPos" "KeyRot" "PosRot" "KeyPosRot")

declare -a combinationsToUse=("PosRot" "KeyPos" "KeyRot" "Pos" "Rot" "Key" "KeyPosRot")

declare -a hiddenLayers=(
	#40 0 0 0 0 #one layer
	20 0 0 0 0
	#10 0 0 0 0
	10 5 0 0 0 #cone down layers
	#20 10 5 0 0
	40 30 20 10 0
	#100 70 50 30 10
	20 60 20 0 0 #out then in layers
	#20 40 80 20 0
)

declare -a hiddenLayerCount=(
	#1
	1
	#1
	2
	#3
	4
	#5
	3
	#4
)

for combination in "${combinationsToUse[@]}"
do
	layerLength=${#hiddenLayerCount[@]}
	for ((i = 0; i<${layerLength}; i++))
	do
		#echo "running setting: " $combination ${hiddenLayerCount[i]} ${hiddenLayers[i*5]} ${hiddenLayers[i*5+1]} ${hiddenLayers[i*5+2]} ${hiddenLayers[i*5+3]} ${hiddenLayers[i*5+4]}
		./master\ Offline\ Tool.exe $combination ${hiddenLayerCount[i]} ${hiddenLayers[i*5]} ${hiddenLayers[i*5+1]} ${hiddenLayers[i*5+2]} ${hiddenLayers[i*5+3]} ${hiddenLayers[i*5+4]}
		./../../Heatmap\ builder/Heatmap\ builder/bin/Debug/Heatmap\ builder.exe "$dataStill $combination ${hiddenLayerCount[i]} ${hiddenLayers[i*5]} ${hiddenLayers[i*5+1]} ${hiddenLayers[i*5+2]} ${hiddenLayers[i*5+3]} ${hiddenLayers[i*5+4]}"
	done
done
