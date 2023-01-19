import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AmbushingAgentsComponent } from './ambushing-agents.component';

describe('AmbushingAgentsComponent', () => {
  let component: AmbushingAgentsComponent;
  let fixture: ComponentFixture<AmbushingAgentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AmbushingAgentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AmbushingAgentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
