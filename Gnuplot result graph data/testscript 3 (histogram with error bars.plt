reset
fontsize = 12


set style fill solid 1.00 border 0
set style histogram errorbars gap 2 lw 1
set style data histogram
set xtics rotate by -45
set grid ytics
set xlabel "Configuration"
set ylabel "Percentage guessed"
set y2label "dummyY2Lable"
set y2tics
set y2range [0:10]

set yrange [0:*]
set datafile separator ","
plot 'layers.txt' using 2:3:xtic(1) ti "percentage correct guess" linecolor rgb "#00FF00", \
'' using 4:5 ti "Percentage guessed lost when not" lt 1 lc rgb "#FF0000" axes x1y2