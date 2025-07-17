<script lang="ts">
  import { page } from "$app/stores";
  import { onMount } from "svelte";
  import axios from "axios";

  let mbid = "";
  $: mbid = $page.params.mbid;

  let albumTitle = "";
  let artist = "";
  let cover = "";
  let tracks: { title: string; length: number }[] = [];
  let albumRating = 0;
  let trackRatings: number[] = [];
  let loading = true;
  let error = "";

  function formatTime(ms: number): string {
    if (!ms) return "--:--";
    const minutes = Math.floor(ms / 60000);
    const seconds = Math.floor((ms % 60000) / 1000);
    return `${minutes}:${seconds.toString().padStart(2, "0")}`;
  }

  function formatRating(r: number): string {
    return r.toFixed(1);
  }

  onMount(async () => {
    loading = true;
    try {
      const groupRes = await axios.get(
        `https://musicbrainz.org/ws/2/release-group/${mbid}`,
        {
          params: { fmt: "json", inc: "artist-credits" },
          headers: { "User-Agent": "YourApp/1.0 (youremail@example.com)" },
        }
      );
      albumTitle = groupRes.data.title;
      artist =
        groupRes.data["artist-credit"]?.[0]?.artist?.name ||
        groupRes.data["artist-credit"]?.[0]?.name ||
        "Неизвестно";
      cover = `https://coverartarchive.org/release-group/${mbid}/front`;

      const releasesRes = await axios.get(
        `https://musicbrainz.org/ws/2/release`,
        {
          params: { "release-group": mbid, fmt: "json" },
          headers: { "User-Agent": "YourApp/1.0 (youremail@example.com)" },
        }
      );
      for (const rel of releasesRes.data.releases) {
        const detail = await axios.get(
          `https://musicbrainz.org/ws/2/release/${rel.id}`,
          {
            params: { inc: "recordings", fmt: "json" },
            headers: { "User-Agent": "YourApp/1.0" },
          }
        );
        const t = detail.data.media?.[0]?.tracks;
        if (t?.length) {
          tracks = t.map((tr: any) => ({
            title: tr.title,
            length: tr.length ?? tr.recording?.length ?? 0,
          }));
          break;
        }
      }
      albumRating = +(Math.random() * 2 + 3).toFixed(1);
      trackRatings = tracks.map(() => +(Math.random() * 2 + 3).toFixed(1));
    } catch (e) {
      error = "Ошибка загрузки альбома или треклиста";
    }
    loading = false;
  });
</script>

<div
  class="min-h-screen bg-gradient-to-br from-zinc-900 via-zinc-950 to-zinc-900 text-zinc-100 px-0 py-0 font-mono flex items-center justify-center"
>
  {#if loading}
    <div class="flex justify-center items-center w-full h-screen">
      <div
        class="animate-spin inline-block w-12 h-12 border-4 border-zinc-500 border-t-zinc-200 rounded-full"
      ></div>
    </div>
  {:else if error}
    <div class="text-center text-red-400 text-xl py-20 w-full">{error}</div>
  {:else}
    <div
      class="w-full max-w-7xl h-[90vh] bg-zinc-800/90 rounded-3xl shadow-2xl p-0 md:p-0 flex flex-col md:flex-row overflow-hidden border border-zinc-700"
    >
      <div
        class="flex flex-col items-center md:items-start md:w-[400px] bg-zinc-900/80 p-10 gap-8 border-r border-zinc-700"
      >
        <img
          src={cover}
          alt="Обложка"
          class="w-52 h-52 md:w-72 md:h-72 object-cover rounded-2xl shadow-lg mb-4"
        />
        <div class="w-full">
          <h1 class="text-4xl font-bold mb-3 leading-tight">{albumTitle}</h1>
          <p class="text-zinc-400 text-2xl mb-6">{artist}</p>
          <div class="flex items-center gap-3 mb-2">
            <span class="text-yellow-400 text-3xl">★</span>
            <span class="font-semibold text-2xl"
              >{formatRating(albumRating)}</span
            >
            <span class="text-zinc-400 text-base ml-2">Album rating</span>
          </div>
        </div>
      </div>
      <div class="flex-1 p-0 flex flex-col h-full max-h-full overflow-y-auto">
        <div class="sticky top-0 z-20 bg-zinc-800 border-b border-zinc-700">
          <h2 class="text-3xl font-semibold mb-0 text-zinc-200 px-10 py-4">
            Tracklist
          </h2>
        </div>
        <ol class="space-y-3 mt-4 px-10">
          {#each tracks as track, idx}
            <li
              class="flex justify-between items-center text-lg md:text-xl text-zinc-300 py-3 px-4 rounded-lg hover:bg-zinc-900/70 transition-colors"
            >
              <span class="flex items-center gap-4">
                <span class="text-zinc-500 w-10 text-right"
                  >{(idx + 1).toString().padStart(2, "0")}</span
                >
                <span class="truncate max-w-[320px] md:max-w-[500px]"
                  >{track.title}</span
                >
              </span>
              <span class="flex items-center gap-3 min-w-[110px] justify-end">
                <span class="text-zinc-500 font-mono"
                  >{formatTime(track.length)}</span
                >
                <span class="text-yellow-400">★</span>
                <span class="text-zinc-400 font-mono"
                  >{formatRating(trackRatings[idx])}</span
                >
              </span>
            </li>
          {/each}
        </ol>
      </div>
    </div>
  {/if}
</div>
