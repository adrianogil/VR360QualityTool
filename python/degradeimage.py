import numpy as np
import cv2

def degradeimage(image, pathtosave, degration_rate=0.8):
    scale = 1 - degration_rate
    small = cv2.resize(image, (0,0), fx=scale, fy=scale)
    # degraded = cv2.resize(small, (0,0), fx=(1/scale), fy=(1/scale))
    degraded = cv2.resize(small, (image.shape[1], image.shape[0]), interpolation = cv2.INTER_CUBIC)
    cv2.imwrite(pathtosave, degraded)

if __name__ == '__main__':
    import sys
    
    if len(sys.argv) == 1:
        ballons = "images/ballons.jpg"
        image = cv2.imread(ballons)
        degradeimage(image, 'images/ballons_degraded.jpg')