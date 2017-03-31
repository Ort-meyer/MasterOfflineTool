reset
set title "dummyTitle"
set xlabel "dummyXLable"
set ylabel "dummyYLable"
set style fill solid 1.0
set boxwidth 0.8

plot 'dummyFileName' every ::0::0 using 1:2 with boxes lc rgb "#00ff0000" title "Mean dummyLineName values of cross validation",\
	'dummyFileName' every ::1 using 1:2 with boxes lc rgb "#0000ff00" notitle