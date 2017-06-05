reset
fontsize = 12

#set title "Play test statistics"
set term pdf
set output "MMORPGPlayTime.pdf"
set style fill solid 1.00 border 0
set style histogram gap 1.2
set style data histogram
set xtics rotate by -45
set grid ytics
set xlabel "Weekly play time"
set ylabel "Participants"
#set y2label "Percentage guessed: false"
#set y2tics
#set y2range [0:80]

#set yrange [0:80]
set datafile separator "."
plot "FrekvensMMORPGSpelande.txt" using 2:xtic(1) notitle linecolor rgb "#00FF00"