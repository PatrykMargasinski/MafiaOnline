import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutBossComponent } from './about-boss.component';

describe('AboutBossComponent', () => {
  let component: AboutBossComponent;
  let fixture: ComponentFixture<AboutBossComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AboutBossComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutBossComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
