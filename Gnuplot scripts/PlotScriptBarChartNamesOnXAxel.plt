reset
set style histogram cluster gap 0

set title "dummyTitle"
set xlabel "dummyXLable"
set ylabel "dummyYLable"
set y2label "dummyY2Lable"
set y2tics

set style fill solid 1.0
set boxwidth 0.5 relative
set yrange [0:100]
set y2range [0:50]

plot 'testdata.txt' using 2:xticlabels(1) with boxes lc rgb "#0000ff00" notitle,\
	'testdata.txt' using 3:xticlabels(1) with boxes lc rgb "#00ff0000" notitle axes x1y2
