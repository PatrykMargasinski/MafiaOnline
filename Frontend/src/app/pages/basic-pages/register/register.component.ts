import { RegisterRequest } from './../../../models/player/player.requests';
import { PlayerService } from './../../../services/player/player.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private playerService: PlayerService) { }

  register(form: NgForm) {
    const credentials : RegisterRequest = {
      'Nick': form.value.nick,
      'Password': form.value.password,
      'BossFirstName': form.value.firstname,
      'BossLastName': form.value.lastname,
      'AgentNames': [form.value.agent1, form.value.agent2, form.value.agent3]
    }
    this.playerService.register(credentials)
    .subscribe(data=>{
      alert("Registration finished. You get 5000$ and 3 agents. That's how your journey begins.");
      this.router.navigate(["/login"]);
    });
  }

  ngOnInit(): void {
  }

}
