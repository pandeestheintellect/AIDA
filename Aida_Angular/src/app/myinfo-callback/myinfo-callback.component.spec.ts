import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyinfoCallbackComponent } from './myinfo-callback.component';

describe('MyinfoCallbackComponent', () => {
  let component: MyinfoCallbackComponent;
  let fixture: ComponentFixture<MyinfoCallbackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyinfoCallbackComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyinfoCallbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
