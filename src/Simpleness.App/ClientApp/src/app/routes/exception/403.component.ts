import { Component } from '@angular/core';
import { NzModalService } from 'ng-zorro-antd';
import { Router } from '@angular/router';

@Component({
  selector: 'exception-403',
  template: `<exception type="403" style="min-height: 500px; height: 80%;">
  <button nz-button [nzType]="'primary'" (click)="goBack()">回到首页</button>
  <button nz-button [nzType]="'warning'" (click)="goLogin()">重新登录</button>
  </exception>`,
})
export class Exception403Component {


  constructor(modalSrv: NzModalService, private router: Router) {
    modalSrv.closeAll();
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
  goLogin(): void {
    this.router.navigate(['/passport/login']);
  }
}
