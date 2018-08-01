using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simpleness.Infrastructure.AspNetCore.Authorize
{
    /// <summary>
    /// 用于Policy 自动注册
    /// </summary>
    public class PermissionAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "Permission";

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        /// <summary>
        /// 自定义Policy
        /// </summary>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {           
                //Create new Policy
                var policy = new AuthorizationPolicyBuilder();
                //基于资源授权
                //policy.AddRequirements(new PermissionRequirement(policyName.Substring(POLICY_PREFIX.Length)));
                //基于Claims 授权
                policy.RequireClaim(POLICY_PREFIX, policyName.Substring(POLICY_PREFIX.Length));

                return Task.FromResult(policy.Build());
            }
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
