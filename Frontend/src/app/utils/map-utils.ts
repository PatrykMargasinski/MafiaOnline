import { Injectable } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Point } from '../models/map/point.models';

@Injectable({
  providedIn: 'root'
})
export class MapUtils {

  constructor() { }

  isRoad(x: number, y: number)
  {
    return x % 6 == 0 || y % 6 == 0;
  }

  isCorner(x: number, y: number)
  {
    return ((x % 6 == 1) && (x % 6 == 5)) || ((y % 6 == 1) && (y % 6 == 5));
  }

  isStreet(x: number, y: number)
  {
    return ((x % 6 == 1 || x % 6 == 5) && y % 6 != 0) ||
    ((y % 6 == 1 || y % 6 == 5) && x % 6 != 0);
  }

  areAdjacent(X1: number, Y1: number, X2: number, Y2: number)
  {
    return ((!(Math.abs(X1-X2)==1 && Math.abs(Y1-Y2)==1)) && (Math.abs(X1-X2)==1 || Math.abs(Y1-Y2)==1))
  }

  clearPath()
  {
    sessionStorage.setItem('path', null);
  }

  getPath()
  {
    let road = sessionStorage.getItem('path');
    var roadArray = JSON.parse(road) as Point[];
    if(roadArray==null) roadArray=[]
    return roadArray
  }

  addPointToPath(x: number, y: number)
  {
    var roadArray = this.getPath()
    let point: Point = {X:x,Y:y}
    roadArray.push(point);
    sessionStorage.setItem('path',JSON.stringify(roadArray));
  }

  removePath(x: number, y: number)
  {
    var roadArray = this.getPath()
    roadArray=roadArray.filter((el) => !(el.X == x && el.Y == y))
    sessionStorage.setItem('path',JSON.stringify(roadArray));
  }

  isPointPath(x: number, y: number)
  {
    var roadArray = this.getPath()
    let point = roadArray.filter((el) => el.X == x && el.Y == y)
    return point.length!=0;
  }

  getFirstElementOfPath()
  {
    var roadArray = this.getPath()
    if(roadArray.length==0) return null;
    return roadArray[0];
  }

  getLastElementOfPath()
  {
    var roadArray = this.getPath()
    if(roadArray.length==0) return null;
    return roadArray[roadArray.length-1];
  }

  pathExists()
  {
    var roadArray = this.getPath()
    return roadArray.length!=0
  }
}
