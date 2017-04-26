#cd Master\ Offline\ Tool/Debug

dataStill=$1
#declare -a combinationsToUse=("Key" "Pos" "Rot" "KeyPos" "KeyRot" "PosRot" "KeyPosRot")

declare -a combinationsToUse=("PosRot")

declare -a hiddenLayers=(
	#40 0 0 0 0 #one layer
	#20 0 0 0 0
	#10 0 0 0 0
	#10 5 0 0 0 #cone down layers
	#20 10 5 0 0
	#40 30 20 10 0
	#100 70 50 30 10
	200 150 100 50 25 10 0
	300 200 150 100 50 30 10
	200 200 200 200 200 0 0
	#20 60 20 0 0 #out then in layers
	#20 40 80 20 0
)

declare -a hiddenLayerCount=(
	#1
	#1
	#1
	#2
	#3
	#4
	#5
	#3
	#4
	6
	7
	5
)

for combination in "${combinationsToUse[@]}"
do
	layerLength=${#hiddenLayerCount[@]}
	for ((i = 0; i<${layerLength}; i++))
	do
		echo "running setting: " $combination ${hiddenLayerCount[i]} ${hiddenLayers[i*7]} ${hiddenLayers[i*7+1]} ${hiddenLayers[i*7+2]} ${hiddenLayers[i*7+3]} ${hiddenLayers[i*7+4]} ${hiddenLayers[i*7+5]} ${hiddenLayers[i*7+6]}
		./master\ Offline\ Tool.exe $combination ${hiddenLayerCount[i]} ${hiddenLayers[i*5]} ${hiddenLayers[i*5+1]} ${hiddenLayers[i*5+2]} ${hiddenLayers[i*5+3]} ${hiddenLayers[i*5+4]} $dataStill
		#./../../Heatmap\ builder/Heatmap\ builder/bin/Debug/Heatmap\ builder.exe "$dataStill $combination ${hiddenLayerCount[i]} ${hiddenLayers[i*5]} ${hiddenLayers[i*5+1]} ${hiddenLayers[i*5+2]} ${hiddenLayers[i*5+3]} ${hiddenLayers[i*5+4]}"
	done
done
