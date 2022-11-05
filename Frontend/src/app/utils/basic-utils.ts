import { Injectable } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class BasicUtils {

  constructor(private sanitized: DomSanitizer) { }

  stringDateConvert(stringDate: string) {
    var d = new Date(stringDate);
    let ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d);
    let mo = new Intl.DateTimeFormat('en', { month: 'short' }).format(d);
    let da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d);
    return `${da}-${mo}-${ye}`
  }

  printMissionPercentagesWithColors(difficulty: number, strength: number, dexterity: number, intelligence: number)
  {
    return this.sanitized.bypassSecurityTrustHtml('Difficulty: '+difficulty+'<br> (<span style="color:orange;">'+strength+'%</span>/<span style="color:darkgreen;">'+dexterity+'%</span>/<span style="color:blue;">'+intelligence+'%</span>)');
  }

}
