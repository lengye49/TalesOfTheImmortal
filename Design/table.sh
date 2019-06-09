cd tools/
if [ $# -eq 0 ]; then
    python table.py
elif [ $# -eq 1 ]; then
    python table.py $1
fi