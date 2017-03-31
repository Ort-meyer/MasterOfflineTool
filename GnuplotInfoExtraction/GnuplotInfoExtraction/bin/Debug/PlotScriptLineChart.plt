reset
set title "dummyTitle"
set xlabel "dummyXLable"
set ylabel "dummyYLable"


plot 'dummyFileName' using 1:2 with lines lc rgb "#00ff0000" title "Mean dummyLineName values of cross validation",\
	for[i=3:20] 'dummyFileName' using 1:i with lines lc rgb "#eeff0000" notitle

	