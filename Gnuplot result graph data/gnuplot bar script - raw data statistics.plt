reset
fontsize = 12

#set title "Play test statistics"
set term pdf
set output "RawDataStatistics.pdf"
set style fill solid 1.00 border 0
set style histogram gap 1.2
set style data histogram
set xtics rotate by -45
set grid ytics
set xlabel "Player"
set ylabel "Frames"
#set y2label "Percentage guessed: false"
#set y2tics
#set y2range [0:80]

#set yrange [0:80]
plot "RawDataStatistics.txt" using 2:xtic(1) title "Frames played" linecolor rgb "#00FF00",\
 "RawDataStatistics.txt" using 3:xtic(1) title "Frames lost" linecolor rgb "#FF0000"
