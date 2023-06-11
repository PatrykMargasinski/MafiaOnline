import { Component, OnInit } from '@angular/core';
import { Job } from 'src/app/models/job/job.models';
import { JobService } from 'src/app/services/job/job.service';

@Component({
  selector: 'app-active-jobs',
  templateUrl: './active-jobs.component.html',
  styleUrls: ['./active-jobs.component.css']
})
export class ActiveJobsComponent implements OnInit {

  constructor(private shared: JobService) { }

  Jobs: Job[]

  ngOnInit(): void {
    this.refreshJobs();
  }

  refreshJobs()
  {
    this.shared.getAllActiveJobs().subscribe(x=>{
      this.Jobs=x;
    })
  }

}
