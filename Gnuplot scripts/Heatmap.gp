
# wxt
set terminal wxt size 350,262 enhanced font 'Verdana,10' persist
set autoscale
set xrange [ -0.5 : 4.5 ] noreverse nowriteback
set yrange [ -0.5 : 4.5 ] noreverse nowriteback
set palette rgbformulae -7, 2, -7
# png
#set terminal pngcairo size 350,262 enhanced font 'Verdana,10'
#set output 'heat_map_interpolation2.png'

#set border linewidth 0
#unset key
#unset colorbox
#unset tics
#set lmargin screen 0.1
#set rmargin screen 0.9
#set tmargin screen 0.9
#set bmargin screen 0.1
#set palette grey

#set pm3d map
#set pm3d interpolate 1,1
#splot 'plotData.txt' using 1:2:3 with pm3d title "4 data columns x/y/z/color"
plot 'plotData.txt' using 1:2:3 with image
reset
