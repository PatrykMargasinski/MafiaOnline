import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseAgentToArrangeAmbushComponent } from './choose-agent-to-arrange-ambush.component';

describe('ChooseAgentToArrangeAmbushComponent', () => {
  let component: ChooseAgentToArrangeAmbushComponent;
  let fixture: ComponentFixture<ChooseAgentToArrangeAmbushComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseAgentToArrangeAmbushComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseAgentToArrangeAmbushComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
