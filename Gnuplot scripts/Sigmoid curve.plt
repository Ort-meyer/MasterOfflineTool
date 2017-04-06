set term png
set output "sigmoid_curve.png"

set xrange [-6:6]
set xzeroaxis linetype -1
set yzeroaxis linetype -1
set xtics axis nomirror
set ytics axis nomirror 0,0.5,1
set key off
set grid
set border 1

set samples 400

plot exp(x)/(1 + exp(x)) with line linetype rgbcolor "red" linewidth 2