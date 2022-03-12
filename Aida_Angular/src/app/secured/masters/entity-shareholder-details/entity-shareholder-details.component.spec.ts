import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntityShareholderDetailsComponent } from './entity-shareholder-details.component';

describe('EntityShareholderDetailsComponent', () => {
  let component: EntityShareholderDetailsComponent;
  let fixture: ComponentFixture<EntityShareholderDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntityShareholderDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityShareholderDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
