import { Injectable } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

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

  getAgentRoad()
  {
    let road = sessionStorage.getItem('agentRoad');
    var roadArray = JSON.parse(road) as number[][];
    if(roadArray==null) roadArray=[]
    return roadArray
  }

  addRoadToAgentRoad(x: number, y: number)
  {
    var roadArray = this.getAgentRoad()
    roadArray.push([x,y]);
    sessionStorage.setItem('agentRoad',JSON.stringify(roadArray));
  }

  removeAgentRoad(x: number, y: number)
  {
    var roadArray = this.getAgentRoad()
    roadArray=roadArray.filter((el) => !(el[0] == x && el[1] == y))
    sessionStorage.setItem('agentRoad',JSON.stringify(roadArray));
  }

  isPointAgentRoad(X: number, Y: number)
  {
    var roadArray = this.getAgentRoad()
    let point = roadArray.filter((x) => x[0] == X && x[1] == Y)
    return point.length!=0;
  }

  getLastElementOfAgentRoad()
  {
    var roadArray = this.getAgentRoad()
    if(roadArray.length==0) return null;
    return roadArray[roadArray.length-1];
  }
}
