import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class BasicUtils {

  constructor(private sanitized: DomSanitizer) { }

  dateFormat(date: Date) {
    const datepipe: DatePipe = new DatePipe('en-US')
    let formattedDate = datepipe.transform(date, 'dd-MMM-yyyy HH:mm:ss')
    return formattedDate;
  }

  printMissionPercentagesWithColors(difficulty: number, strength: number, dexterity: number, intelligence: number)
  {
    return this.sanitized.bypassSecurityTrustHtml('Difficulty: '+difficulty+'<br> (<span style="color:orange;">'+strength+'%</span>/<span style="color:darkgreen;">'+dexterity+'%</span>/<span style="color:blue;">'+intelligence+'%</span>)');
  }

  isPlayerNotActivated()
  {
    return sessionStorage.getItem("notActivated")=='1';
  }

}
