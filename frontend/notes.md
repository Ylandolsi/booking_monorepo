useEffect(() => {
const observer = new IntersectionObserver(
([entry]) => {
if (entry.isIntersecting) {
setIsInView(true);
observer.disconnect();
}
},
{ threshold: 0.1 },
);

    if (imgRef.current) {
      observer.observe(imgRef.current);
    }

    return () => observer.disconnect();

}, []);
