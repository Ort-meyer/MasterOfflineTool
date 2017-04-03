reset

set title "Some Title"
set xlabel "Name of network"
set ylabel "Correct guess"
set y2label "Guessed lost when not"

set style fill solid 1.0
set boxwidth 0.5 

set style line 1 lc rgb "red"
set style line 2 lc rgb "blue"

set xtics ("label" 0.25, "label2" 1.75, "bar label" 3.25,)


plot 'testdata.txt' every 2 using 1:2 with boxes lc 1,\
	'testdata.txt' every 2::1 using 1:2 with boxes lc 2

