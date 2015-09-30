#!/usr/bin/env python
#coding=utf-8
import matplotlib.image as mpimg
from pylab import *
from mpl_toolkits.basemap import Basemap
import numpy as np
import matplotlib.pyplot as plt
import os, errno

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
    for i in iFile:
        line = np.array([float(field) for field in i.strip().lstrip('[').rstrip(']').split()],dtype=float)
        matrix.append(line)
    iFile.close()
    return matrix

def Draw_Isoline( inText, outImg, color, levs, dimLon, dimLat ):
    #print dimLat[1]-dimLat[2], dimLat[0], dimLon[0], dimLon[1]+dimLon[2]

    map = Basemap(projection='cyl',llcrnrlat=-90, urcrnrlat=90,
        llcrnrlon=0, urcrnrlon=360)

    data = openFile(inText)

    lon = make_lon(dimLon[0], dimLon[1], dimLon[2], len(data))
    lat = make_lat(dimLat[0], dimLat[1], dimLat[2], len(data[0]))

    #print len(lat), len(lat[0]), len(lon), len(lon[0])
    
    x, y = map(lon, lat)
   
    map.fillcontinents(color='#FFFCC4')
    #map.drawparallels(np.arange(dimLat[1]-dimLat[2], dimLat[0], abs(dimLat[2])), labels=[1,0,0,0], color="grey",fontsize=6) # draw parallels
    #map.drawmeridians(np.arange(dimLon[0], 360-abs(dimLon[0]), abs(dimLon[2])), labels=[0,0,0,1], color="grey",fontsize=5) # draw meridians
    map.drawcoastlines(linewidth=0.2)
    CS = plt.contour(x, y, data, levs, colors=color)
    plt.clabel(CS, inline=1, inline_spacing=0.1, fontsize=7, fmt='%1.3f', colors='b')

    plt.savefig(outImg + ".png", dpi=500, pad_inches=0.05, bbox_inches='tight')


def main():
    inText = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Sources\\temp\\in\\pavelgloba.txt"
    inText = sys.argv[1]
    outImg = "D:\\Projects\\GeoSpace\\GeoSpaceTrack\\Sources\\temp\\out\\img.png"

    outImg = sys.argv[2]

    last = float(sys.argv[3])
    step = float(sys.argv[4])

    levs = np.arange(0.0, last, step)

    #START END STEP
    dimLon = [0, 361, 1]
    dimLat = [-90, 91, 1]

    Draw_Isoline( inText, outImg, "black", levs, dimLon, dimLat )
if __name__ == '__main__':
    main()