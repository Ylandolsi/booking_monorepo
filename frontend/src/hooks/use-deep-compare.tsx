import { useEffect, useRef, type DependencyList, type EffectCallback } from 'react';
import { deepEqual } from '@/lib/deep-equal';

export function useDeepCompareEffect(effect: EffectCallback, dependencies: DependencyList): void {
  const previousDeps = useRef<DependencyList>();

  // Check deep equality
  const isSame = previousDeps.current ? deepEqual(previousDeps.current, dependencies) : false;

  if (!isSame) {
    previousDeps.current = dependencies;
  }

  useEffect(effect, [previousDeps.current]);
}

// Usage Example:
// const [filters, setFilters] = useState({ category: 'books', price: 20 });

// useDeepCompareEffect(() => {

//   // ✅ cleanup function example
//   return () => {
//   };
// }, [filters]);
