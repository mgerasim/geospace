#!/usr/bin/env python
#coding=utf-8
import matplotlib.image as mpimg
from pylab import *
from mpl_toolkits.basemap import Basemap
import numpy as np
import matplotlib.pyplot as plt
import os, errno
from os import path

def make_lon( startLat, endLat, stepLat, dimY):
    matrix = []
    line = np.arange(startLat, endLat, stepLat)
    #print "line", line
    for i in range(dimY):
        matrix.append(line)
    matrix = np.array(matrix)
    return matrix


def make_lat(startLon, endLon, stepLon, dimX):    
    matrix = []    
    coord = np.arange(startLon, endLon, stepLon)
    #print "coord", coord
    for num in coord:                
        matrix.append([num for i in range(dimX)])
    matrix = np.array(matrix)
    #print matrix
    return matrix


def openFile(inText):
    iFile = open(inText, 'r')
    matrix = []
    #for i in range(5):
    #    s = iFile.readline()
    for i in iFile:
        line = np.array([float(field) for field in i.strip().lstrip('[').rstrip(']').split()],dtype=float)
        matrix.append(line)
    iFile.close()
    return np.array( matrix )

def Draw_Isoline( inText, inText2, outImg, color, levs, dimLon, dimLat ):
    #print dimLat[1]-dimLat[2], dimLat[0], dimLon[0], dimLon[1]+dimLon[2]

    map = Basemap(projection='cyl',llcrnrlat=-90, urcrnrlat=90,
        llcrnrlon=0, urcrnrlon=360)

    data = openFile(inText)
    #data1 = openFile(inText2)
    #data2 = data - data1
    #print len(data), len(data[0])

    lon = make_lon(dimLon[0], dimLon[1], dimLon[2], len(data))
    lat = make_lat(dimLat[0], dimLat[1], dimLat[2], len(data[0]))

    print len(lat), len(lat[0]), len(lon), len(lon[0])
    
    x, y = map(lon, lat)
   
    map.fillcontinents(color='#FFFCC4')
    #map.drawparallels(np.arange(dimLat[1]-dimLat[2], dimLat[0], abs(dimLat[2])), labels=[1,0,0,0], color="grey",fontsize=6) # draw parallels
    #map.drawmeridians(np.arange(dimLon[0], 360-abs(dimLon[0]), abs(dimLon[2])), labels=[0,0,0,1], color="grey",fontsize=5) # draw meridians
    map.drawcoastlines(linewidth=0.2)
    CS = plt.contour(x, y, data, levs, colors=color)
    plt.clabel(CS, inline=1, inline_spacing=0.1, fontsize=7, fmt='%1.3f', colors='b')

    plt.savefig(outImg + ".png", dpi=500, pad_inches=0.05, bbox_inches='tight')


def main():
    print "Python"
    inText = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Sources\\temp\\in\\pavelgloba.txt"
    outImg = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Sources\\temp\\out\\img.png"
    new_directory_path = "X:\\incoming\\vsavchenko\\ver2\\new_modip\\sep\\"
    directory_path = "X:\\incoming\\vsavchenko\\ver2\\modip\\sep\\"
    Fout_path = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Sources\\temp\\out\\F"
    F3out_path = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Sources\\temp\\out\\F3"
    Mout_path = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Sources\\temp\\out\\M"

    inText = sys.argv[1]
    outImg = sys.argv[2]

    last = float(sys.argv[3])
    step = float(sys.argv[4])
    levs = np.arange(0.0, last, step)

    print outImg, inText, last, step

    #START END STEP
    dimLon = [0, 361, 1]
    dimLat = [90, -91, -1]
    Draw_Isoline( inText, "", outImg, "black", levs, dimLon, dimLat )
    plt.clf()

    return

    files = [x for x in os.listdir(directory_path) if path.isfile(directory_path+os.sep+x)]

    for f in files:
        if ".grd" in f:
            print f

            last = float(sys.argv[3])
            step = float(sys.argv[4])


            inText = directory_path + f           
            inText2 = new_directory_path + f    


            if "f" in f:
                outImg = Fout_path
                last = 1.0
                step = 0.1
                
            if "M" in f:
                outImg = Mout_path
                last = 1.0
                step = 0.1

            if "F3" in f:
                outImg = F3out_path


            if "100" in f:
                outImg = outImg + "_100\\" + f[:-4]
            else: 
                outImg = outImg + "_0\\" + f[:-4]

            print outImg
            
            levs = np.arange(0.0, last, step)

            #START END STEP
            dimLon = [0, 360, 1]
            dimLat = [-90, 91, 1]

            Draw_Isoline( inText, inText2, outImg, "black", levs, dimLon, dimLat )
            plt.clf()
if __name__ == '__main__':
    main()