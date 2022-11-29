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

  clearAgentPath()
  {
    sessionStorage.setItem('agentPath', null);
  }

  getAgentPath()
  {
    let road = sessionStorage.getItem('agentPath');
    var roadArray = JSON.parse(road) as number[][];
    if(roadArray==null) roadArray=[]
    return roadArray
  }

  addRoadToAgentPath(x: number, y: number)
  {
    var roadArray = this.getAgentPath()
    roadArray.push([x,y]);
    sessionStorage.setItem('agentPath',JSON.stringify(roadArray));
  }

  removeAgentPath(x: number, y: number)
  {
    var roadArray = this.getAgentPath()
    roadArray=roadArray.filter((el) => !(el[0] == x && el[1] == y))
    sessionStorage.setItem('agentPath',JSON.stringify(roadArray));
  }

  isPointAgentPath(X: number, Y: number)
  {
    var roadArray = this.getAgentPath()
    let point = roadArray.filter((x) => x[0] == X && x[1] == Y)
    return point.length!=0;
  }

  getLastElementOfAgentPath()
  {
    var roadArray = this.getAgentPath()
    if(roadArray.length==0) return null;
    return roadArray[roadArray.length-1];
  }

  agentPathExists()
  {
    var roadArray = this.getAgentPath()
    return roadArray.length!=0
  }
}
