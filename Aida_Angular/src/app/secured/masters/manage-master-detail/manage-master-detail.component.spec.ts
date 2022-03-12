import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageMasterDetailComponent } from './manage-master-detail.component';

describe('ManageMasterDetailComponent', () => {
  let component: ManageMasterDetailComponent;
  let fixture: ComponentFixture<ManageMasterDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageMasterDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageMasterDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
