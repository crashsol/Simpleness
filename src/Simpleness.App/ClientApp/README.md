#### 使用ng-alain构建后台管理系统
- 创建项目
- 项目整体配置，修改 environments/environments.ts中的配置信息
  ```
  export const environment = {
  SERVER_URL: `http://localhost:5006/api/`,
  production: false,
  useHash: false,
  hmr: false,
}; 
  ```
- 启用JwtTokenInteceptor.修改app.module.ts中的
  ```
 
  const INTERCEPTOR_PROVIDES = [
  //{ provide: HTTP_INTERCEPTORS, useClass: SimpleInterceptor, multi: true},
  { provide: HTTP_INTERCEPTORS, useClass: JWTInterceptor, multi: true },
   
  { provide: HTTP_INTERCEPTORS, useClass: DefaultInterceptor, multi: true}

   ```
- 由于我们使用HttpStatusCode来标记请求的结果，所以需要修改app/core/net/default.interceptor.ts
  ```
  default:
  // 统一抛出异常
  if (event instanceof HttpErrorResponse) {
    console.warn(event.error);
    this.msg.error(event.error);
    return throwError({});
  }
  break;
  ```
  
- 修改app/routes/login/loginComponents.ts中的登录逻辑
- 在进行注册的时候发现，ng-alain默认JwtokenInterceptor会验证Token，如果请求注册地址，需要通过配置文件添加请求白名单
  ```
    return Object.assign(new DelonAuthConfig(), <DelonAuthConfig>{
    login_url: '/passport/login',
    // 添加过滤，不验证Token的Url白名单
     ignores: [/account\//, /assets\//, /passport\//]
  });
  ```
  
