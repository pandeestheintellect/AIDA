import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SecretariesDetailComponent } from './secretaries-detail.component';

describe('SecretariesDetailComponent', () => {
  let component: SecretariesDetailComponent;
  let fixture: ComponentFixture<SecretariesDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SecretariesDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SecretariesDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
