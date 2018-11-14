import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'passport-register-result',
  templateUrl: './register-result.component.html'
})
export class UserRegisterResultComponent implements OnInit {

  status = false;
  email = '';
  constructor(
    public msg: NzMessageService,
    private router: ActivatedRoute
  ) { }


  ngOnInit(): void {
    this.router.queryParams.subscribe(params => {
      this.status = params['status'];
      this.email = params['email'];
    });
  }
}
