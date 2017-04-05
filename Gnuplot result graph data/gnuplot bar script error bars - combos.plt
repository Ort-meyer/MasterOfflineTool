# Gnuplot script that plots bars for true and false positives with error bars

reset
fontsize = 12

set term svg
set output "combos.svg"
set style fill solid 1.00 border 0
set style histogram errorbars gap 2 lw 1
set style data histogram
set xtics rotate by -45
set grid ytics
set xlabel "Configuration"
set ylabel "Percentage guessed: true"
set y2label "Percentage guessed: false"
set y2tics
set y2range [0:80]

set yrange [0:80]
set datafile separator "."
plot 'combos.txt' using 2:3:xtic(1) ti "True positive" linecolor rgb "#00FF00", \
'' using 4:5 ti "False positive" lt 1 lc rgb "#FF0000" axes x1y2