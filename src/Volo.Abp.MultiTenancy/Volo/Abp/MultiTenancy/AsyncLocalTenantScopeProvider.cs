using System;
using System.Threading;
using Volo.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class AsyncLocalTenantScopeProvider : ITenantScopeProvider, IScopedDependency
    {
        public TenantScope CurrentScope
        {
            get { return _currentScope.Value; }
            private set { _currentScope.Value = value; }
        }

        private readonly AsyncLocal<TenantScope> _currentScope;

        public AsyncLocalTenantScopeProvider()
        {
            _currentScope = new AsyncLocal<TenantScope>();
        }

        public IDisposable EnterScope(TenantInfo tenantInfo)
        {
            var parentScope = CurrentScope;
            CurrentScope = new TenantScope(tenantInfo);
            return new DisposeAction(() =>
            {
                CurrentScope = parentScope;
            });
        }
    }
}