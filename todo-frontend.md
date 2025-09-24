- update store info similat to create form
- create / update product
- drag and drop
- user overview and book
- maybe add statistic
- adjust booking ...

- [ ] Review of toast- especially failures
- [ ] review keys of tanstack
- [ ] replace all list ids with generated cyrpto ids
- [ ] make sure of this works : for availability : dont show today's slots when time has passed : example :today at 15 don show me slots from 9 to 14
- [ ] add : naviagte to mentor or mentee when showing the meets
- [ ] Booking page: improve mobile version.
- [ ] check all time in front / back and make sure they are okay : ( check experience , education : explicitly )
- [ ] fix timing in getSession : and use ToLocalIsoString everywhere and use the same datetiomeFormat
- [ ] covnert date that came from backend as UTC to localTimezone before format it (24 sep 2024 ) ...
- [ ] email verification flow
- [ ] become mentor backend check
- [ ] improve the flow of u cant book unless integrated
- [ ] add this condition to backend as well

---

- [ ] policy management UI react
- [ ] group types that realted to the api calls to that api

### wrap api with this to observe the time !!

```ts
export default function fetcher(f: () => Promise<any>, key: unknown) {
  return async () => {
    const start = performance.now();
    const r = await f();
    const end = performance.now();
    console.log(`${JSON.stringify(key)}: ${end - start}ms`);
    return r;
  };
}
```

### single flight to avoid race !!

```ts
type AsyncFunction<V> = () => Promise<V>;

const pendingPromises: Map<string, Promise<any>> = new Map();

const run = async <V>(key: string, fn: AsyncFunction<V>): Promise<V> => {
  if (pendingPromises.has(key)) {
    return pendingPromises.get(key) as Promise<V>;
  }

  const promise = fn();
  pendingPromises.set(key, promise);

  try {
    const result = await promise;
    return result;
  } finally {
    pendingPromises.delete(key);
  }
};

export default {
  run,
};
```

---

### how to use tanstack query efficeyvely

- Codegen from an openapi spec using orval.dev. This generated all the api code as well as mock data. The setting we used was api functions rather than hooks

- Used the query key factory : https://github.com/lukemorales/query-key-factory#fine-grained-declaration-colocated-by-features so we can co locate query keys and api calls by feature. This centralised both query keys and apis calls in one place

- separated api layers . this idea: https://profy.dev/article/react-architecture-api-layer

- As implied above. used feature folders to isolate codebase by feature

* All react query fetches were done inside custom hooks

* follow pattern used here as well : https://github.com/maffin-io/maffin-app/blob/master/src/hooks/api/useMonthlyWorth.ts

```ts
    queryFn: fetcher(
      () => getMonthlyTotals(
        accounts as Account[],
        interval,
      ),
      queryKey,
    ),

        select: aggregate,
    networkMode: 'always',

```

- read more about select and network mode
  fetcher :

```ts
export default function fetcher(f: () => Promise<any>, key: unknown) {
  return async () => {
    const start = performance.now();
    const r = await f();
    const end = performance.now();
    console.log(`${JSON.stringify(key)}: ${end - start}ms`);
    return r;
  };
}
```

### document query selectors :

````ts
 useEffect(() => {
        // Prevent background scrolling when modal is open
        const originalOverflow = document.body.style.overflow;
        document.body.style.overflow = "hidden";

        // Also prevent scrolling on any overflow-auto containers
        const scrollableContainers = document.querySelectorAll(
            '.overflow-auto, [style*="overflow: auto"], [style*="overflow-auto"]'
        );
        const originalContainerOverflows: string[] = [];

        scrollableContainers.forEach((container, index) => {
            const element = container as HTMLElement;
            originalContainerOverflows[index] = element.style.overflow;
            element.style.overflow = "hidden";
        });

        // Cleanup: restore original overflow when component unmounts
        return () => {
            document.body.style.overflow = originalOverflow;
            scrollableContainers.forEach((container, index) => {
                const element = container as HTMLElement;
                element.style.overflow = originalContainerOverflows[index];
            });
        };
    }, []);
    ```
````

--

### Selecting the parent With useRef :

````ts
    const modalRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const parent = modalRef.current?.parentElement;
        console.log("parent", parent);
        if (!parent) return;

        // Prevent background scrolling when modal is open
        const originalOverflow = parent.style.overflow;
        parent.style.overflow = "hidden";

        // Cleanup: restore original overflow when component unmounts
        return () => {
            parent.style.overflow = originalOverflow;
        };
    }, []);

    return (
        <div
            ref={modalRef}
            className="absolute inset-0 top-[-20px] z-50 flex items-center justify-center bg-background/50 backdrop-blur-sm"
        >
        ```
````

---

### How to use forward ref

````ts
type WelcomeProps = {
  message?: string;
  onClick?: () => void;
};

const WelcomeToTheNextLeague = React.forwardRef<HTMLDivElement, WelcomeProps>(
  ({ message = "Welcome to the Next League", onClick }, ref) => {
    ```
````

```spacing :
tracking-wider" style={{ letterSpacing: '0.0.5em' }}
```

const error = new URLSearchParams(location.search).get('error') ?? undefined;
form.setValue('dailySchedule', newSchedule, { shouldValidate: true });

---
