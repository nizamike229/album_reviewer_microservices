<script lang="ts">
  export let rating: number = 0;
  export let onChange: (value: number) => void;
  export let readOnly: boolean = false;

  let hoverRating = 0;
  const stars = [1, 2, 3, 4, 5];

  function handleMouseMove(event: MouseEvent, star: number) {
    if (!readOnly) {
      const rect = (event.currentTarget as HTMLElement).getBoundingClientRect();
      const x = event.clientX - rect.left;
      hoverRating = x < rect.width / 2 ? star - 0.5 : star;
    }
  }

  function handleMouseLeave() {
    if (!readOnly) hoverRating = 0;
  }

  function handleClick(event: MouseEvent, star: number) {
    if (!readOnly) {
      const rect = (event.currentTarget as HTMLElement).getBoundingClientRect();
      const x = event.clientX - rect.left;
      const value = x < rect.width / 2 ? star - 0.5 : star;
      rating = value;
      onChange(value);
    }
  }
</script>

<div
  class="flex items-center gap-3"
  role="radiogroup"
  tabindex="0"
  on:mouseleave={handleMouseLeave}
>
  {#each stars as star}
    <button
      type="button"
      class="w-10 h-10 cursor-pointer flex items-center justify-center relative group"
      class:cursor-default={readOnly}
      on:mousemove={(e) => handleMouseMove(e, star)}
      on:click={(e) => handleClick(e, star)}
      disabled={readOnly}
      aria-label="Rate {star} {star === 1 ? 'star' : 'stars'}"
      role="radio"
      aria-checked={rating === star}
    >
      <div class="relative w-10 h-10">
        <div class="absolute inset-0 text-4xl leading-none flex">
          <div class="w-1/2 overflow-hidden">
            <span
              class="transition-colors block w-full text-center"
              class:text-yellow-400={(hoverRating || rating) >= star - 0.5}
              class:text-zinc-600={(hoverRating || rating) < star - 0.5}>★</span
            >
          </div>
          <div class="w-1/2 overflow-hidden">
            <span
              class="transition-colors block w-full text-center"
              class:text-yellow-400={(hoverRating || rating) >= star}
              class:text-zinc-600={(hoverRating || rating) < star}>★</span
            >
          </div>
        </div>
      </div>
    </button>
  {/each}
  <span class="ml-2 text-zinc-400">{(hoverRating || rating).toFixed(1)}</span>
</div>
