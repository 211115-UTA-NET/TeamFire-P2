import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TradesearchComponent } from './tradesearch.component';

describe('TradesearchComponent', () => {
  let component: TradesearchComponent;
  let fixture: ComponentFixture<TradesearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TradesearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TradesearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
